package handlers

import (
	"Server/db"
	"fmt"
	"net/http"

	"github.com/gin-gonic/gin"
	"github.com/google/uuid"
)

func Register(c *gin.Context) {
	id := c.PostForm("id")
	password := c.PostForm("password")
	username := c.PostForm("username")

	userUUID := uuid.New().String()
	fmt.Println("value: ", id, password, username)
	if db.DB == nil {
		fmt.Println("value: 연결끊김")
		c.JSON(http.StatusInternalServerError, gin.H{"status": "error", "message": "Database not initialized"})
		return
	}

	// 비밀번호 해시 처리 후 DB에 저장 mysql ver
	// _, err := db.DB.Exec("INSERT INTO users (username, password, email) VALUES (?, ?, ?)", id, password, email)
	// if err != nil {
	// 	c.JSON(http.StatusInternalServerError, gin.H{"status": "error", "message": "Failed to register"})
	// 	return
	// }

	// 비밀번호 해시 처리 후 DB에 저장 postgres
	_, err := db.DB.Exec("INSERT INTO auth (id, userid, username, userpassword) VALUES ($1, $2, $3, $4)", userUUID, id, username, password)
	if err != nil {
		fmt.Println("value: 연결끊김")
		c.JSON(http.StatusInternalServerError, gin.H{"status": "error", "message": "Failed to register"})
		return
	}

	// 성공적으로 저장되었을 때
	c.JSON(http.StatusOK, gin.H{"status": "success", "message": "Registration successful"})

}
