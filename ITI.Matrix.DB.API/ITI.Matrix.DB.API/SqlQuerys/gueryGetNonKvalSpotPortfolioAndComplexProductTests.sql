 select /*+ index(p PV_IDCLIENT_IDX)*/p.id_portfolio,
	  case nvl(c.clgroup_id,0)
		  when 0 then null
		  when 1 then -100
		  when 13 then -1
		  when 3 then 0
		  when 4 then 4
		  when 5 then 5
		  when 6 then 6
		  when 7 then 7
		  when 8 then 8
		  when 9 then 9
		  when 10 then 10
		  when 11 then 11
		  when 12 then 800
		  when 14 then 16
		  when 15 then 512
	  end quik_code
from moff.ACL_CLIENT2GROUP c, moff.portfolio_leverage p
  where c.ps_code=p.id_person
	and p.id_portfolio=p.id_account
	and c.clgroup_id>2
	and c.clgroup_id<16
	and c.ps_code in 
		(select ps_code from moff.persons 
			where trade_system = 'Q')
	and p.secboard != 'RTS_FUT'