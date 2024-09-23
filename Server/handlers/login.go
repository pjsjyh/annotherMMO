package handlers

import (
	"Server/db"
	"net/http"

	"github.com/gin-gonic/gin"
)

func Login(c *gin.Context) {
	id := c.PostForm("id")
	password := c.PostForm("password")

	var storedPassword string
	err := db.DB.QueryRow("SELECT password FROM users WHERE username = ?", id).Scan(&storedPassword)
	if err != nil {
		c.JSON(http.StatusUnauthorized, gin.H{"status": "error", "message": "Invalid username or password"})
		return
	}

	if password != storedPassword {
		c.JSON(http.StatusUnauthorized, gin.H{"status": "error", "message": "Invalid username or password"})
		return
	}

	c.JSON(http.StatusOK, gin.H{"status": "success", "message": "Login successful"})
}
