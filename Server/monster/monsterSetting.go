package monster

import (
	"database/sql"

	_ "github.com/lib/pq"
)

type Monster struct {
	MonsterInfoID    string
	MonsterID        string
	PositionX        int
	PositionY        int
	PositionZ        int
	LastResponseTime int
	IsActive         bool
}

func MonsterSetting(db *sql.DB) ([]Monster, error) {
	query := `
        SELECT monsterinfo_id, monster_id, position_x, position_y, position_z, last_response_time, is_active
        FROM monsterinfo
        WHERE is_active = TRUE;
    `

	rows, err := db.Query(query)
	if err != nil {
		return nil, err
	}
	defer rows.Close()

	var monsters []Monster
	for rows.Next() {
		var monster Monster
		err := rows.Scan(&monster.MonsterInfoID, &monster.MonsterID, &monster.PositionX, &monster.PositionY, &monster.PositionZ, &monster.LastResponseTime, &monster.IsActive)
		if err != nil {
			return nil, err
		}
		monsters = append(monsters, monster)
	}
}
