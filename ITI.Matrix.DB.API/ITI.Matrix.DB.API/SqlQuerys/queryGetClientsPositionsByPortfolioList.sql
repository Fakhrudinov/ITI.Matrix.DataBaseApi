select 	p.id_cl_portf PORTFOLIO, 
		p.seccode, 
		cast(p.amount as number(38,7)) AMOUNT, 
		cast(p.w_avr_price as number(38,7)) AV_PRICE, 
		p.secboard BOARD, 
		case cp.acc_ext when '689' then cp.alias else cp.acc_ext end as TKS 
	from moff.portfolios p 
	RIGHT join moff.client_portfolio cp  on p.id_cl_portf = cp.id 
		WHERE p.ID_CL_PORTF IN ( :portfoliosList ) 