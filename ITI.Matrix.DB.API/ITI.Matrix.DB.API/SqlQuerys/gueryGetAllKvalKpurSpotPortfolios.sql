SELECT ID_ACCOUNT  
FROM  moff.client_portfolio t, moff.persons p
WHERE t.id_client=p.ps_code
   and id_dealing = 1    
   AND p.trade_system = 'Q'
   AND secboard in ('EQ', 'CETS')
   AND t.ismargin>2 
   AND p.ps_code not in (select ps from moff.a0_nerezy) 
   AND t.ID_ACCOUNT NOT LIKE '%-CD-%'
