package main

import (
	"context"
	"fmt"
	"net/http"

	"github.com/gin-gonic/gin"
	"github.com/go-redis/redis/v8"
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
