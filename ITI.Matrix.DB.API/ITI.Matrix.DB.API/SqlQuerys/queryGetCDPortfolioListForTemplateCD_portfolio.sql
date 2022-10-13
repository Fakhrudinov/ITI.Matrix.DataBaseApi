select ID_ACCOUNT from v_kval_risk_stat 
	WHERE 
		rez_status != 'UNFRIEND_NEREZ'
		AND ID_ACCOUNT LIKE '%-CD-%'			
		AND ID_ACCOUNT not LIKE 'BC68409-%'