select 
		cp.id AS CL_CODE, 
		cast(cp.cash as number(38,7)) AS LIMLIM,  
		cp.ismargin AS LEVERAGE 
	from moff.client_portfolio cp
	where cp.id like :account || '-%'	
		and cp.secboard != 'RTS_FUT' 