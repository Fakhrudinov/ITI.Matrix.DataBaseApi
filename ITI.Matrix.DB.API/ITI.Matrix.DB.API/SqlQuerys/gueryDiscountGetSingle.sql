select nvl(t.discount, 0) DISC, t.is_short IS_SHORT , t.tiker
	from moff.securities t 
		where t.tiker = :security