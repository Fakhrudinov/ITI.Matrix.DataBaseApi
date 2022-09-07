select p.alias 
from moff.ACL_CLIENT2GROUP t, moff.client_portfolio p
  where t.ps_code=p.id_client
	  and p.secboard='RTS_FUT'
	  and t.clgroup_id=16
	  and p.alias is not null
