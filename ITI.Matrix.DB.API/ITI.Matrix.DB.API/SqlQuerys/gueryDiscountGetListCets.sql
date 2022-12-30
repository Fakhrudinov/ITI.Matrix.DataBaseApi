select nvl (t.discount, 0) DISC, t.is_short IS_SHORT, t.tiker
	from moff.securities t 
		where t.tiker in 
			('USD', 'USD000UTSTOM', 'CNY', 'CNYRUB_TOM', 'EUR', 'EUR_RUB__TOM', 'GBP', 'GBPRUB_TOM')