select ID_ACCOUNT from v_kval_risk_stat
	WHERE 
		rez_status = 'FRIEND_NEREZ' 
		AND (
			ID_ACCOUNT LIKE '%-CD-%'
			)
		AND ALIAS != 'BC68409'