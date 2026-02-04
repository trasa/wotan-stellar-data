\copy (select id, system_name, system_type, source_system_id, star_mass, star_spectral_type_size, distance_sol_ly, star_radius, star_luminosity from star_systems) TO '/Users/trasa/prj/wotan-stellar-data/star_systems.csv' WITH CSV HEADER;

\copy (select * from stars) TO '/Users/trasa/prj/wotan-stellar-data/stars.csv' WITH CSV HEADER;
