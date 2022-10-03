select ALIAS
	from moff.V_QUIK_QUIZ_RESULTS
		WHERE quiz_board = 'FUT' 
		AND ALIAS IS NOT NULL
