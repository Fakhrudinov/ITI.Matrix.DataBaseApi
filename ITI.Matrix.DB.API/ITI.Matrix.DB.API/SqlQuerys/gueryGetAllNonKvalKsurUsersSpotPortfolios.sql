SELECT ID_ACCOUNT  
FROM  moff.client_portfolio t, moff.persons p
WHERE t.id_client=p.ps_code
   and id_dealing IS null         
   AND p.trade_system = 'Q'
   AND secboard in ('EQ', 'CETS')
   AND t.ID_ACCOUNT NOT LIKE '%-CD-%'
   and t.ismargin<=2
   and p.ps_code not in (select ps from moff.a0_nerezy)
   AND t.ID_ACCOUNT NOT in ('BP19181-FX-01', 'BC65970-FX-30')
