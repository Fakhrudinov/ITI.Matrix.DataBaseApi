select t.discount DISC, t.is_short IS_SHORT , t.tiker  
	from moff.securities t 
		where 	t.ismargin = 1  
			and t.status in ( 'A', 'B')
			and t.secboard = 'EQ'