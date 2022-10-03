select ALIAS from v_kval_risk_stat
	WHERE 		
		rez_status ='UNFRIEND_NEREZ'
		AND ID_ACCOUNT LIKE '%-RF-%'