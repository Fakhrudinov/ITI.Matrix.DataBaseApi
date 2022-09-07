SELECT ID_ACCOUNT FROM  moff.client_portfolio t 
	WHERE t.ID_CLIENT IN 
	(select ps from moff.a0_nerezy n, moff.persons p 
		where n.ps=p.ps_code 
		and not_friend =1  
		and p.trade_system='Q') 
	AND t.secboard= 'CETS' 
	AND t.ID_ACCOUNT LIKE '%-CD-%'