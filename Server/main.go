package main

import (
	"Server/db"
	"Server/handlers"

	"github.com/gin-gonic/gin"
)

func main() {
	r := gin.Default()

	// 데이터베이스 초기화
	db.InitDB()
	// fmt.Printf("DB address in main: %p\n", db.DB)

	// 라우트 설정
	r.POST("/register", handlers.Register)
	r.POST("/login", handlers.Login)

	// 서버 시작
	r.Run(":8080")
}
