
CREATE TABLE moons
(
    id                  SERIAL PRIMARY KEY,
    planet_id           INTEGER          NOT NULL REFERENCES planets (id) ON DELETE CASCADE,
    moon_name           VARCHAR(255)     NOT NULL,
    moon_type           VARCHAR(50)      NOT NULL,
    orbital_radius_km   DOUBLE PRECISION NOT NULL,
    orbital_period_days DOUBLE PRECISION NOT NULL,
    mass_earth_masses   DOUBLE PRECISION NOT NULL,
    radius_earth_radii  DOUBLE PRECISION NOT NULL,
    surface_type        VARCHAR(100),
    seed                INTEGER          NOT NULL,
    created_at          TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
CREATE INDEX idx_moons_planet ON moons(planet_id);
