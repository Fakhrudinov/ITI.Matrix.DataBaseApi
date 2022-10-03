select ID_ACCOUNT from v_kval_risk_stat
	WHERE 		
		rez_status = 'REZIDENT'
		AND KVAL = 0
		AND RISK = 'KSUR'		
		AND
		(
			ID_ACCOUNT LIKE '%-MS-%'
			OR ID_ACCOUNT LIKE '%-FX-%'
		)
		AND ID_ACCOUNT NOT in 
			('BP19181-FX-01', 'BC65970-FX-30')