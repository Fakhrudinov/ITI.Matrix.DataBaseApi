select ID_ACCOUNT from v_kval_risk_stat
	WHERE 
		rez_status = 'UNFRIEND_NEREZ'
		AND (
			ID_ACCOUNT LIKE '%-MS-%' 
			OR ID_ACCOUNT LIKE '%-FX-%'
			)
		OR ID_ACCOUNT LIKE 'BC68409-MS-%'
		OR ID_ACCOUNT LIKE 'BC68409-FX-%'
