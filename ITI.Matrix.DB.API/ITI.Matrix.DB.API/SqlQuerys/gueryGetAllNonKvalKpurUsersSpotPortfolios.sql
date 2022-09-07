SELECT ID_ACCOUNT  
FROM  moff.client_portfolio t, moff.persons p
WHERE t.id_client=p.ps_code
   and id_dealing IS null         
   AND p.trade_system = 'Q'
   AND secboard in ('EQ', 'CETS')
   AND t.ID_ACCOUNT NOT LIKE '%-CD-%'
   and t.ismargin>2
   and p.ps_code not in 
   	(select ps from moff.a0_nerezy n where n.not_friend=1)
