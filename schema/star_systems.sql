CREATE TABLE star_systems (
    id SERIAL PRIMARY KEY NOT NULL,
    system_name VARCHAR(255) NOT NULL,
    system_type VARCHAR(255) NOT NULL,
    source_system_id VARCHAR(255) NOT NULL,
    galactic_x DOUBLE PRECISION NOT NULL,
    galactic_y DOUBLE PRECISION NOT NULL,
    galactic_z DOUBLE PRECISION NOT NULL,
    position GEOMETRY(POINTZ, 0) NOT NULL,
    star_mass DOUBLE PRECISION NULL,
    star_spectral_type_size VARCHAR(255) NULL,
    distance_sol_ly DOUBLE PRECISION NOT NULL,
    star_radius DOUBLE PRECISION NULL,
    star_luminosity DOUBLE PRECISION NULL
);

CREATE INDEX idx_star_systems_position ON star_systems USING GIST (position);
CREATE INDEX idx_star_systems_distance ON star_systems (distance_sol_ly);
CREATE INDEX idx_star_systems_name ON star_systems (system_name);
