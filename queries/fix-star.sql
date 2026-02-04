/* 
@STAR_IDS-- the stars.id values which belong to one system
@MULTIPLE_STAR_SYSTEM - the star_systems.id that the stars SHOULD belong to
@DUPLICATE_STAR_SYSTEM_IDS - the erroneous 'star systems' that just represent one star in a multiple-star system.
*/

/* get the star_system_id for the stars that should belong to the same star_system_id */
SELECT @DUPLICATE_STAR_SYSTEM_IDS = star_system_id FROM stars WHERE id IN (@STAR_IDS);

/* Update those stars to refer to the correct 'multiple' system id */
UPDATE stars SET star_system_id = @MULTIPLE_STAR_SYSTEM WHERE id IN (@STAR_IDS);


/* eliminate the duplicate star system IDS - stars should not reference them anymore */
DELETE star_systems where id in @DUPLICATE_STAR_SYSTEM_IDS;

