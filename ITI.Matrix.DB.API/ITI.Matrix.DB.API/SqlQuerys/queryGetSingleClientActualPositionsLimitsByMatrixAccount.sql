select
		p.id_cl_portf AS CL_CODE, 
		p.seccode, cast(p.amount as number(38,7)) AS AMOUNT,  
		cast(p.w_avr_price as number(38,7)) AS AV_PRICE,  
		p.secboard AS BOARD, 
		case cp.acc_ext when '689' then cp.alias else cp.acc_ext end AS TKS 
	from moff.portfolios p  
	inner join moff.client_portfolio cp  on p.id_cl_portf = cp.id 
		where p.id_cl_portf like :account || '-%'
		and p.secboard != 'RTS_FUT'