INSERT INTO stars (
    star_system_id,
    star_name,
    star_type,
    source_system_id,
    galactic_x, galactic_y, galactic_z,
    position,
    star_mass,
    star_spectral_type_size,
    distance_sol_ly,
    star_radius,
    star_luminosity
)
SELECT 
    id, 
    system_name,
    system_type,
    source_system_id,
    galactic_x, galactic_y, galactic_z,
    position,
    star_mass,
    star_spectral_type_size,
    distance_sol_ly,
    star_radius,
    star_luminosity
FROM star_systems
WHERE system_type != 'Multiple';

