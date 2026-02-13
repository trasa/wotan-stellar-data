CREATE TABLE planetary_rings (
                                 id BIGSERIAL PRIMARY KEY,
                                 planet_id BIGINT NOT NULL REFERENCES planetary_ring_systems(planet_id) ON DELETE CASCADE,
                                 ring_index INTEGER NOT NULL,
                                 inner_radius DOUBLE PRECISION NOT NULL,  -- in planet radii
                                 outer_radius DOUBLE PRECISION NOT NULL,  -- in planet radii
                                 density DOUBLE PRECISION NOT NULL CHECK (density BETWEEN 0 AND 1),
                                 brightness DOUBLE PRECISION NOT NULL CHECK (brightness BETWEEN 0 AND 1),
                                 ring_type VARCHAR(20) NOT NULL,
                                 tint_color_r INTEGER NOT NULL CHECK (tint_color_r BETWEEN 0 AND 255),
                                 tint_color_g INTEGER NOT NULL CHECK (tint_color_g BETWEEN 0 AND 255),
                                 tint_color_b INTEGER NOT NULL CHECK (tint_color_b BETWEEN 0 AND 255),
                                 seed INTEGER NOT NULL,
                                 UNIQUE(planet_id, ring_index)
);

CREATE INDEX idx_rings_planet ON planetary_rings(planet_id);
