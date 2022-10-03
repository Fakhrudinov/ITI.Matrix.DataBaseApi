select ID_ACCOUNT from v_kval_risk_stat
	WHERE 
		rez_status = 'FRIEND_NEREZ' 
		AND KVAL = 1
		AND (
			ID_ACCOUNT LIKE '%-MS-%' 
			OR ID_ACCOUNT LIKE '%-FX-%'
			)
		AND ALIAS != 'BC68409'