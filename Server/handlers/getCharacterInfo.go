package handlers

import (
	"Server/db"
)

type GetPlayerInfo struct {
	ID        string
	Character string `json:"character"`
	Skill     string `json:"skill"`
	Auth_id   string
	Username  string
}

func GetCharacterInfo(id, username string) GetPlayerInfo {
	var playerInfo GetPlayerInfo
	err := db.DB.QueryRow("SELECT id, character, skill, auth_id FROM playerinfo WHERE auth_id = $1", id).Scan(&playerInfo.ID, &playerInfo.Character, &playerInfo.Skill, &playerInfo.Auth_id)
	if err != nil {
		return playerInfo
	}
	playerInfo.Username = username
	// var characterData CharacterInfo
	// var skillData SkillInfo

	// json.Unmarshal([]byte(playerInfo.Character), &characterData) // JSON -> 구조체
	// json.Unmarshal([]byte(playerInfo.Skill), &skillData)

	// fmt.Println(characterData)
	// fmt.Println(skillData)

	//  response := map[string]interface{}{
	//     "character": characterData,
	//     "skill":     skillData,
	// }

	// // 응답을 JSON으로 변환
	// jsonResponse, err := json.Marshal(response)
	// if err != nil {
	//     log.Fatal(err)
	// }

	// // 클라이언트로 JSON 응답 보내기
	// fmt.Fprintln(w, string(jsonResponse))  // 'w'는 http.ResponseWriter
	// playerInfo 구조체를 JSON으로 변환
	return playerInfo
}