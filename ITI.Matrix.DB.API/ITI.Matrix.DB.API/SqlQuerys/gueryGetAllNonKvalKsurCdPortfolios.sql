select ID_ACCOUNT from v_kval_risk_stat
	WHERE 		
		rez_status = 'REZIDENT'
		AND KVAL = 0
		AND RISK = 'KSUR'		
		AND ID_ACCOUNT LIKE '%-CD-%'