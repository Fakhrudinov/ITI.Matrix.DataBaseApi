SELECT ID_ACCOUNT  
FROM  moff.client_portfolio t, moff.persons p
WHERE t.id_client=p.ps_code
   AND p.trade_system = 'Q'
   AND secboard= 'CETS'
   AND t.ID_ACCOUNT LIKE '%-CD-%'
