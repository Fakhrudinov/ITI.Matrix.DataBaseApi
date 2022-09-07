select id_sec_type,tiker from moff.securities
where
   secboard in ('EQ')
   and not 
  	(substr(isin,1,2) in ('RU','SU')
   or tiker in (select seccode from moff.noqual_seccode))
   AND id_sec_type NOT IN ('EQRP', 'EQ', 'RPMO')
UNION ALL
SELECT 'EQRP' AS id_sec_type, '<ALL>' AS tiker FROM DUAL
UNION ALL
SELECT 'RPMO' AS id_sec_type, '<ALL>' AS tiker FROM DUAL
