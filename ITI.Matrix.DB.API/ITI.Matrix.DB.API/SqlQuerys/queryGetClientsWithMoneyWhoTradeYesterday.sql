select cp.id CL_CODE, cast(cp.cash as number(38,7)) LIMLIM, cp.ACC_EXT AS TKS
	from moff.client_portfolio cp 
	where cp.id_client in 
		( 
			select distinct d.id_client 
			from moff.dealing d 
				where d.date_fill between trunc (sysdate) - :dayShiftOne and trunc (sysdate) - :dayShiftTwo 
				and d.id_client in 
				(
					select prs.ps_code 
					from moff.persons prs 
						where prs.trade_system= 'Q' 
						and prs.ps_code not in ('BP19195', 'BC645824')
				)
				and d.secboard != 'RTS_FUT' 
		) 
		and cp.secboard != 'RTS_FUT' 
		AND cp.cash != 0