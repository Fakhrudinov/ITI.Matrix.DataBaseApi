select
		cp.id AS CL_CODE, 
		case substr (cp.id, INSTR(cp.id, '-') + 1, 2)
			when 'MS' then 'ABRD' 
			when 'FX' then 'USD000UTSTOM' 
			when 'CD' then 'USD000UTSTOM' 
			when 'RS' then 'MMM' 
			when 'RF' then 'VBH' || substr(extract(year from sysdate) + 1, -1)
			WHEN 'SF' THEN 'REGAL1' || substr(extract(year from sysdate) + 1, -1) 
			else 'GAZP' end AS SECCODE, 
		0 AS AMOUNT,  
		0 AS AV_PRICE, 
		cp.secboard AS BOARD, 
		case cp.acc_ext when '689' then cp.alias else cp.acc_ext end AS TKS 
	from moff.client_portfolio cp 
	where cp.id like :account || '-%'
		and substr (cp.id, INSTR(cp.id, '-') + 1, 2) != 'MO'  
		and cp.secboard != 'RTS_FUT'