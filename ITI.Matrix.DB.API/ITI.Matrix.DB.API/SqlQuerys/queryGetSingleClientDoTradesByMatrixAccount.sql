SELECT 1 AS Result
	from moff.dealing 
	WHERE ID_CL_ACC LIKE :account || '-%'	
		AND SECBOARD != 'RTS_FUT' 
		AND DATE_FILL between trunc ( sysdate - :dayShiftOne ) and trunc (sysdate - :dayShiftTwo) 
		AND ROWNUM <= 1