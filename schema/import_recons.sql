CREATE TEMP TABLE recons_temp (
    system_type VARCHAR(255) NOT NULL,
    system_id VARCHAR(255) NOT NULL,
    system_name VARCHAR(255) NOT NULL,
    galactic_x DOUBLE PRECISION NOT NULL,
    galactic_y DOUBLE PRECISION NOT NULL,
    galactic_z DOUBLE PRECISION NOT NULL,
    mass DOUBLE PRECISION NULL,
    blank1 VARCHAR(50) NULL,
    blank2 VARCHAR(50) NULL,
    spectral_type VARCHAR(255) NULL,
    blank3 VARCHAR(50) NULL,
    distance_ly DOUBLE PRECISION NOT NULL
);

\COPY recons_temp FROM '/Users/trasa/prj/wotan-stellar-data/GAL_RECONS.csv' DELIMITER ',' CSV; 

INSERT INTO star_systems (
    system_name, 
    system_type, 
    source_system_id, 
    galactic_x, 
    galactic_y, 
    galactic_z, 
    position, 
    star_mass, 
    star_spectral_type_size, 
    distance_sol_ly)
SELECT 
    system_name,
    system_type,
    system_id,
    galactic_x, galactic_y, galactic_z,
    ST_MakePoint(galactic_x, galactic_y, galactic_z),
    mass,
    spectral_type,
    distance_ly
FROM recons_temp;

