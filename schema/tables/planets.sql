CREATE TABLE planets
(
    id                    SERIAL PRIMARY KEY,
    star_id               INTEGER          NOT NULL REFERENCES stars (id) ON DELETE CASCADE,
    planet_name           VARCHAR(255)     NOT NULL,
    planet_type           VARCHAR(50)      NOT NULL, -- terrestrial, gas_giant, ice_giant, dwarf, etc.
    orbital_radius_au     DOUBLE PRECISION NOT NULL, -- distance from star in AU
    orbital_period_days   DOUBLE PRECISION NOT NULL,
    mass_earth_masses     DOUBLE PRECISION NOT NULL,
    radius_earth_radii    DOUBLE PRECISION NOT NULL,
    surface_temperature_k DOUBLE PRECISION,
    atmosphere_type       VARCHAR(100),              -- none, thin, thick, toxic, etc.
    surface_type          VARCHAR(100),              -- rocky, icy, gaseous, molten, etc.
    seed                  INTEGER          NOT NULL, -- for deterministic regeneration
    created_at            TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_planets_star ON planets(star_id);
