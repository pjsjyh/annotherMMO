package main

import (
	"context"
	"crypto/rand"
	"encoding/hex"
	"fmt"
	"net/http"
	"time"

	"github.com/gin-gonic/gin"
	"github.com/go-redis/redis/v8"
	"golang.org/x/crypto/bcrypt"
)

var ctx = context.Background()

func main() {
	r := gin.Default()

	// Redis 클라이언트 생성
	rdb := redis.NewClient(&redis.Options{
		Addr:     "localhost:6379", // Redis 주소
		Password: "",               // Redis 비밀번호 (없을 경우 빈 문자열)
		DB:       0,                // Redis 데이터베이스 번호
	})

	r.POST("/register", func(c *gin.Context) {
		id := c.PostForm("id")
		password := c.PostForm("password")

		// 먼저 해당 ID가 이미 존재하는지 확인
		exists, err := rdb.HExists(ctx, "user:"+id, "id").Result()
		if err != nil {
			c.JSON(http.StatusInternalServerError, gin.H{"status": "error", "message": "Failed to check existing ID"})
			return
		}

		if exists {
			c.JSON(http.StatusConflict, gin.H{"status": "error", "message": "ID already exists"})
			return
		}

		// 비밀번호를 해시 처리하여 저장 (오류 처리 포함)
		hashedPassword, err := hashPassword(password)
		if err != nil {
			c.JSON(http.StatusInternalServerError, gin.H{"status": "error", "message": "Failed to hash password"})
			return
		}

		// Redis에 사용자 정보 저장
		err = rdb.HSet(ctx, "user:"+id, map[string]interface{}{
			"id":       id,
			"password": hashedPassword,
		}).Err()
		if err != nil {
			c.JSON(http.StatusInternalServerError, gin.H{"status": "error", "message": err.Error()})
			return
		}

		// 성공 응답
		c.JSON(http.StatusOK, gin.H{"status": "success", "id": id})
	})

	// Redis에 데이터 저장 API
	r.POST("/set", func(c *gin.Context) {
		key := c.PostForm("id")
		value := c.PostForm("password")
		fmt.Println("Key:", key)
		fmt.Println("Value:", value)
		err := rdb.Set(ctx, key, value, 0).Err()
		if err != nil {
			c.JSON(http.StatusInternalServerError, gin.H{"status": "error", "message": err.Error()})
			return
		}

		c.JSON(http.StatusOK, gin.H{"status": "success"})
	})

	r.POST("/login", func(c *gin.Context) {
		fmt.Println("Login request received")
		id := c.PostForm("id")
		password := c.PostForm("password")
		fmt.Println("IDdddd:", id, "Password:", password)
		// Redis에서 사용자 정보 조회
		storedPassword, err := rdb.HGet(ctx, "user:"+id, "password").Result()
		if err == redis.Nil {
			fmt.Println("Login request error1")

			c.JSON(http.StatusUnauthorized, gin.H{"status": "error", "message": "Invalid user ID"})
			return
		} else if err != nil {
			fmt.Println("Login request error2")
			c.JSON(http.StatusInternalServerError, gin.H{"status": "error", "message": err.Error()})
			return
		}

		// 비밀번호 확인
		validPassword, err := checkPassword(password, storedPassword)
		if err != nil || !validPassword {
			fmt.Println("Login request error3")

			c.JSON(http.StatusUnauthorized, gin.H{"status": "error", "message": "Invalid password"})
			return
		}

		// 세션 토큰 생성 및 저장
		sessionToken, err := generateSessionToken()
		if err != nil {
			fmt.Println("Login request error4")

			c.JSON(http.StatusInternalServerError, gin.H{"status": "error", "message": "Failed to generate session token"})
			return
		}

		err = rdb.HSet(ctx, "session:"+id, map[string]interface{}{
			"token":   sessionToken,
			"expires": time.Now().Add(24 * time.Hour).Unix(),
		}).Err()
		if err != nil {
			fmt.Println("Login request error5")

			c.JSON(http.StatusInternalServerError, gin.H{"status": "error", "message": err.Error()})
			return
		}
		// 성공 응답
		c.JSON(http.StatusOK, gin.H{"status": "success", "token": sessionToken})
	})

	// Redis에서 데이터 조회 API
	r.GET("/get", func(c *gin.Context) {
		key := c.Query("key")

		val, err := rdb.Get(ctx, key).Result()
		if err != nil {
			c.JSON(http.StatusInternalServerError, gin.H{"status": "error", "message": err.Error()})
			return
		}

		c.JSON(http.StatusOK, gin.H{"status": "success", "value": val})
	})

	// 서버 시작
	r.Run(":8080")
}

func hashPassword(password string) (string, error) {
	// bcrypt를 사용해 비밀번호 해시 생성 (비용: 14는 bcrypt의 기본 설정)
	bytes, err := bcrypt.GenerateFromPassword([]byte(password), 14)
	return string(bytes), err
}

// 비밀번호를 확인하는 함수
func checkPassword(providedPassword, storedPassword string) (bool, error) {
	err := bcrypt.CompareHashAndPassword([]byte(storedPassword), []byte(providedPassword))
	if err != nil {
		return false, err // 비밀번호 불일치 또는 에러 발생
	}
	return true, nil // 비밀번호 일치
}

// 세션 토큰을 생성하는 함수
func generateSessionToken() (string, error) {
	tokenBytes := make([]byte, 32)
	_, err := rand.Read(tokenBytes)
	if err != nil {
		return "", err // 에러가 발생한 경우 빈 토큰과 에러 반환
	}

	sessionToken := hex.EncodeToString(tokenBytes)
	return sessionToken, nil // 정상적인 토큰 생성
}
