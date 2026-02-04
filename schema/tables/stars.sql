CREATE TABLE stars (
    id SERIAL PRIMARY KEY NOT NULL,
    star_system_id INTEGER NOT NULL REFERENCES star_systems(id) ON DELETE CASCADE,
    star_name VARCHAR(255) NOT NULL,
    star_type VARCHAR(255) NOT NULL,
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

CREATE INDEX idx_stars_position ON stars USING GIST (position);
CREATE INDEX idx_stars_distance ON stars (distance_sol_ly);
CREATE INDEX idx_stars_name ON stars (star_name);
