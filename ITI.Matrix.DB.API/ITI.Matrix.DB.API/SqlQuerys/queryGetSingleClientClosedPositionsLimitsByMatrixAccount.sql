select
		d.id_cl_acc AS CL_CODE, 
		d.seccode, 
		max(cast(nvl(p.amount, '0') as number(38,7))) AS AMOUNT,  
		max(cast(nvl(p.w_avr_price, '0') as number(38,7))) AS AV_PRICE, 
		d.secboard AS BOARD,  
		case cp.acc_ext when '689' then cp.alias else cp.acc_ext end AS TKS 
	from moff.dealing d 
	left join moff.portfolios p on p.id_cl_portf = d.id_cl_acc  and d.seccode = p.seccode 
	left join moff.client_portfolio cp  on d.id_cl_acc = cp.id 
		where d.date_fill between trunc ( sysdate - :dayShiftOne ) and sysdate
			and d.id_cl_acc like :account || '-%'
			and p.amount is null  
			and d.secboard NOT IN ( 'RTS_FUT', 'PMX', 'SPBEX' )
		Group by d.seccode, d.secboard, d.id_cl_acc, cp.ismargin, cp.alias, cp.acc_ext 