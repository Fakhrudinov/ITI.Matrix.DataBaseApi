select SV_AG_ID  
	from moff.services  
		where SV_SL_ID = 394
			AND SV_DATE_FINISH IS NULL
			AND SV_AG_ID = :clientCode 