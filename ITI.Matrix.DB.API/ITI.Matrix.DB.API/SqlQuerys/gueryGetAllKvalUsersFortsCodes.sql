SELECT alias 
FROM moff.client_portfolio 
	WHERE id_dealing = 1 
	AND secboard = 'RTS_FUT' 
	AND ALIAS IS NOT NULL