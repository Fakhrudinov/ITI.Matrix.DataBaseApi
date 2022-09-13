SELECT ID_ACCOUNT  
FROM  moff.client_portfolio t, moff.persons p
WHERE t.id_client=p.ps_code
   and id_dealing = 1
   AND p.trade_system = 'Q'
   AND secboard= 'CETS'
   and t.ismargin<=2 
   and p.ps_code not in (select ps from moff.a0_nerezy)
   AND t.ID_ACCOUNT LIKE '%-CD-%'