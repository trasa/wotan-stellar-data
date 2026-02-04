CREATE TEMP TABLE ctiopi_temp (
    system_type VARCHAR(255) NOT NULL,
    system_id VARCHAR(255) NOT NULL,
    system_name VARCHAR(255) NOT NULL,
    galactic_x DOUBLE PRECISION NOT NULL,
    galactic_y DOUBLE PRECISION NOT NULL,
    galactic_z DOUBLE PRECISION NOT NULL,
    mass DOUBLE PRECISION NULL,
    radius DOUBLE PRECISION NULL,
    luminosity DOUBLE PRECISION NULL,
    spectral_type VARCHAR(255) NULL,
    blank1 VARCHAR(50) NULL,
    distance_ly DOUBLE PRECISION NOT NULL
);

\COPY ctiopi_temp FROM '/Users/trasa/prj/wotan-stellar-data/GAL_CTIOPI_v2.csv' DELIMITER ',' CSV; 

INSERT INTO star_systems (
    system_name, 
    system_type, 
    source_system_id, 
    galactic_x, 
    galactic_y, 
    galactic_z, 
    position, 
    star_mass, 
    star_radius,
    star_luminosity,
    star_spectral_type_size, 
    distance_sol_ly)
SELECT 
    system_name,
    system_type,
    system_id,
    galactic_x, galactic_y, galactic_z,
    ST_MakePoint(galactic_x, galactic_y, galactic_z),
    mass,
    radius,
    luminosity,
    spectral_type,
    distance_ly
FROM ctiopi_temp WHERE system_name != 'Sol';

