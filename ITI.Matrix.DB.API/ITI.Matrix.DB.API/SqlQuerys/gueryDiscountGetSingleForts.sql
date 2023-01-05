select t.discount DISC, t.is_short IS_SHORT , substr(t.EXCODE, 1, 2) || '*' TIKER
	from  moff.securities t 
		where t.secboard = 'RTS_FUT' 
			and t.id_sec_type = 'FUT' 
			and t.status != 'S' 
			and substr(t.short_name, -3) != 'CLT'  
			and substr(t.short_name, -3) != 'RSK' 
			and t.curr_base is null 
			and rownum = 1 
			and substr(t.EXCODE, 1, 2) = substr(:security, 1, 2)