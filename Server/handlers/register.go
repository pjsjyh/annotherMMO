package handlers

import (
	"Server/db"
	"fmt"
	"net/http"

	"github.com/gin-gonic/gin"
	"github.com/google/uuid"
	"golang.org/x/crypto/bcrypt"
)

func Register(c *gin.Context) {
	id := c.PostForm("id")
	password := c.PostForm("password")
	username := c.PostForm("username")

	// 1. DB에서 중복 ID 확인
	var existingID string
	err := db.DB.QueryRow("SELECT userid FROM auth WHERE userid = $1", id).Scan(&existingID)
	if err == nil {
		// 이미 해당 id가 존재하는 경우
		c.JSON(http.StatusConflict, gin.H{"status": "error",
			"message":    "ID already exists",
			"error_type": "duplicate_id",
			// 구체적인 오류 타입 추가
		})
		return
	}
	// 1-1. DB에서 중복 Username 확인
	var existingNAME string
	err = db.DB.QueryRow("SELECT username FROM auth WHERE username = $1", username).Scan(&existingNAME)
	if err == nil {
		// 이미 해당 id가 존재하는 경우
		c.JSON(http.StatusConflict, gin.H{"status": "error",
			"message":    "Username already exists",
			"error_type": "duplicate_username",
			// 구체적인 오류 타입 추가
		})
		return
	}

	// 2. 비밀번호 해시 처리
	hashedPassword, err := bcrypt.GenerateFromPassword([]byte(password), bcrypt.DefaultCost)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"status": "error", "message": "Failed to hash password"})
		return
	}

	// 3. DB에 저장
	userUUID := uuid.New().String()
	//fmt.Println("value: ", id, hashedPassword, username)
	if db.DB == nil {
		fmt.Println("value: 연결끊김")
		c.JSON(http.StatusInternalServerError, gin.H{"status": "error", "message": "Database not initialized"})
		return
	}
	// 비밀번호 해시 처리 후 DB에 저장 postgres
	_, err = db.DB.Exec("INSERT INTO auth (id, userid, username, userpassword) VALUES ($1, $2, $3, $4)", userUUID, id, username, hashedPassword)
	if err != nil {
		fmt.Println("value: 연결끊김")
		c.JSON(http.StatusInternalServerError, gin.H{"status": "error", "message": "Failed to register"})
		return
	}

	// 성공적으로 저장되었을 때
	c.JSON(http.StatusOK, gin.H{"status": "success", "message": "Registration successful"})

}
