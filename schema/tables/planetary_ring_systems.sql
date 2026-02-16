CREATE TABLE planetary_ring_systems (
                                        id BIGINT PRIMARY KEY REFERENCES planets(id),
                                        inner_edge DOUBLE PRECISION NOT NULL,  -- in planet radii
                                        outer_edge DOUBLE PRECISION NOT NULL,  -- in planet radii
                                        primary_composition VARCHAR(20) NOT NULL,
                                        total_mass DOUBLE PRECISION,  -- kg
                                        seed INTEGER NOT NULL
);
