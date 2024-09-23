package db

import (
	"database/sql"
	"fmt"
	"log"

	_ "github.com/go-sql-driver/mysql"
	_ "github.com/lib/pq"
)

var DB *sql.DB

func InitDB() {
	var err error
	// MySQL 연결 설정 (사용자, 비밀번호, 호스트, 포트, 데이터베이스 이름)
	//DB, err = sql.Open("mysql", "root:password@tcp(127.0.0.1:3306)/mygame")

	//postgres
	dsn := "host=localhost port=5432 user=postgres password=000105 dbname=mygame sslmode=disable"
	DB, err := sql.Open("postgres", dsn)
	if err != nil {
		log.Fatal("Failed to connect to the database:", err)
	}

	// 연결 확인
	err = DB.Ping()
	if err != nil {
		log.Fatal("Database connection failed:", err)
	}
	fmt.Println("Database connection successful!")
	fmt.Println("DB object:", DB) // 초기화된 DB 객체 출력
	fmt.Printf("DB address in db: %p\n", DB)

}
