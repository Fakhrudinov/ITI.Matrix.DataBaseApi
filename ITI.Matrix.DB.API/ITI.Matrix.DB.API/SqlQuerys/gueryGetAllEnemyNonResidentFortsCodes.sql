SELECT t.ALIAS FROM  moff.client_portfolio t 
	WHERE t.ID_CLIENT IN 
		(select ps from moff.a0_nerezy n, moff.persons p 
			where n.ps=p.ps_code 
			and not_friend =1) 
	AND t.secboard = 'RTS_FUT'
	AND t.ALIAS IS NOT NULL
