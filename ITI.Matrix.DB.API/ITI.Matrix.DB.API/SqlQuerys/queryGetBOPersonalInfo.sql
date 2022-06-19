select p.CL_TYPE, p.DATE_ENTRY, pbo.ps_address_reg, pbo.ps_ndfl 
from moff.persons p 
LEFT JOIN itbo11.persons@boffce_moff_link pbo 
ON p.KNOW_TYPE = pbo.pS_id 
where p.PS_CODE = :clientCode