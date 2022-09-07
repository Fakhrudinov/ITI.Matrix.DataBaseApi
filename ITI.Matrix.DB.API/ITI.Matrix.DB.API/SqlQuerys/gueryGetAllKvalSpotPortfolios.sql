select distinct id_account 
      from moff.client_portfolio 
      where id_dealing = 1 
        and (id_client in 
				(select ps_code from moff.persons 
					where trade_system = 'Q')
				OR  id_account in 
				('BC21398-MO-03','BC21398-MO-05','BC21398-MO-06'))			
        and secboard in ('CETS', 'EQ')
