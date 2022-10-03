select ALIAS from v_kval_risk_stat
	WHERE 		
		KVAL = 1
		AND ID_ACCOUNT LIKE '%-RF-%'
		AND ALIAS IS NOT NULL