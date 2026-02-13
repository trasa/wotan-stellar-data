update planets set planet_type = case
    when planet_type = 'dwarf_planet' then 'DwarfPlanet'
    when planet_type = 'frozen_terrestrial' then 'FrozenTerrestrial'
    when planet_type = 'gas_giant' then 'GasGiant'
    when planet_type = 'lava_world' then 'LavaWorld'
    when planet_type = 'hot_terrestrial' then 'HotTerrestrial'
    when planet_type = 'ice_giant' then 'IceGiant'
    when planet_type = 'ocean_world' then 'OceanWorld'
    when planet_type = 'terrestrial' then 'Terrestrial'
    else planet_type
end
